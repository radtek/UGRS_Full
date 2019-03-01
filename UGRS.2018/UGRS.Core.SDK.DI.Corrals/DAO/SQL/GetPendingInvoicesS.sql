SELECT T0.CardCode     [Code], 
T0.CardName            [Name], 
ISNULL(T1.Total,0)+ (ISNULL(T2.Total,0)*ISNULL(T3.Price,0))*(1+ CASE WHEN ISNULL(T4.Rate,0) > 0 THEN T4.Rate/100 ELSE 0 END) [Debt],
CASE WHEN ISNULL(T1.Total,0)+ (ISNULL(T2.Total,0)*ISNULL(T3.Price,0))*(1+ Case when ISNULL(T4.Rate,0) >0 then T4.Rate/100 else 0 end) = 0 THEN 'SI' ELSE 'NO' END [Invoiced]
FROM OCRD T0 
-- OBTIENE LOS DOCUMENTOS DE ENTREGAS PENDIENTES DE PAGO
LEFT JOIN (SELECT SUM(B0.DocTotal) Total, B0.CardCode 
	        FROM ODLN B0
	        INNER JOIN (SELECT A0.docentry FROM DLN1 A0 
			            WHERE A0.WhsCode= 'CRHE'
	                    GROUP BY A0.docentry) B1 ON B0.DocEntry = B1.DocEntry
			WHERE B0.CANCELED = 'N' AND B0.DocStatus='O'
			GROUP BY B0.CardCode 
			) T1 on T1.CardCode = T0.CardCode
-- OBTIENE LA EXISTENCIA Y LAS FECHAS
LEFT JOIN (
		  SELECT 
				Q0.CardCode, 
				SUM(CASE WHEN Q0.InvoiceDate IS NOT NULL THEN 
					DATEDIFF(DAY,Q0.InvoiceDate,Q0.DateTrans) 
				ELSE 
					DATEDIFF(DAY,Q0.CreateLote,Q0.DateTrans) +1  
				END * Q0.Quantity)   Total
			FROM 
				(SELECT 
				   W0.BatchNum, W0.quantity,W0.DateTrans,W0.CreateLote,W0.cardcode,
				   (SELECT MAX(R0.DocDate) InvoiceDate 
										FROM OINV R0 
										INNER JOIN INV1 R1 ON R0.DocEntry= R1.DocEntry    
										WHERE R0.CANCELED = 'N' AND R1.U_SU_BatchAuc = W0.BatchNum AND
												R1.ItemCode = (SELECT U_Value FROM [@UG_CONFIG] WHERE "Name" = 'CU_GLO_ITEMPAYR') AND R1.DocDate < W0.DateTrans AND U_PE_Origin = 'S'
					) InvoiceDate
				FROM
					(SELECT 
						SUM(P5.Quantity) Quantity, P5.BatchNum, P5.DocDate DateTrans, P6.inDate CreateLote, P6.MnfSerial CardCode
					FROM OWTR P0
					INNER JOIN WTR1 P1 ON P0.Docentry= P1.Docentry
					INNER JOIN OWHS P2 ON P2.WhsCode = P1.FromWhsCod
					INNER JOIN OLCT P4 ON P4.Code = P2.Location AND P4.Location = 'CRHE'
					INNER JOIN OITM T4 ON P1.ItemCode = T4.ItemCode AND t4.ItmsGrpCod = (select u_value from [@UG_CONFIG] where name ='GLO_ITEMGANADO')
					INNER JOIN IBT1 P5 ON P5.BaseLinNum= P1.LineNum AND P5.BaseEntry = P1.DocEntry AND P5.BaseType = 67  AND P5.WhsCode ='SUHE'
					INNER JOIN OBTN P6 ON P6.DistNumber= P5.BatchNum AND P6.ItemCode = P1.ItemCode
					WHERE P0.U_GLO_STATUS <> 'C'
					GROUP BY  P5.BatchNum, P5.DocDate,P6.inDate, p6.MnfSerial) W0
				) Q0
				GROUP BY Q0.CardCode
	) T2 ON T2.CardCode = T0.CardCode
-- OBTIENE EL PRECIO POR EL ARTICULO DE COBRO DE PISO
LEFT JOIN (
	SELECT A1.ItemCode, A1.Price FROM OPLN A0
	INNER JOIN ITM1 A1 ON A0.ListNum = A1.PriceList
	WHERE --A0.U_GLO_WhsCode='CRHE' 
	 A0.U_GLO_Location = 'CRHE'
	AND a1.ItemCode = (SELECT U_Value FROM [@UG_CONFIG] WHERE "Name" = 'CU_GLO_ITEMPAYR')
) T3 ON 1=1
-- OBTIENE EL EL IMPUESTO APLICADO AL ARTICULO DE COBRO DE PISO
LEFT JOIN (
	SELECT A1.Rate FROM OTCX A0
	INNER JOIN OSTC A1 ON A0.LnTaxCode = A1.Code
	WHERE BUSAREA =0 AND StrVal1 = (SELECT U_Value FROM [@UG_CONFIG] WHERE Name='CU_GLO_ITEMPAYR')
) T4 on 1=1
WHERE T2.CardCode IS NOT NULL OR T1.CardCode IS NOT NULL

