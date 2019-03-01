SELECT
	 T0.CardCode [Code],
	 T0.CardName [Name], 
	 ISNULL(T1.Total,0)+ (ISNULL(T2.Total,0)*ISNULL(T3.Price,0))*(1+ CASE WHEN ISNULL(T4.Rate,0) >0 THEN T4.Rate/100 ELSE 0 END) [Debt],
     CASE WHEN ISNULL(T1.Total,0)+ (ISNULL(T2.Total,0)*ISNULL(T3.Price,0))*(1+ CASE WHEN ISNULL(T4.Rate,0) >0 THEN T4.Rate/100 ELSE 0 end) = 0 THEN 'SI' ELSE 'NO' END [Invoiced]
FROM OCRD T0 
-- OBTIENE LOS DOCUMENTOS DE ENTREGAS PENDIENTES DE PAGO
LEFT JOIN (SELECT SUM(B0.DocTotal) Total, B0.CardCode 
	        FROM ODLN B0
	        INNER JOIN (SELECT A0.docentry FROM DLN1 A0 
	            WHERE A0.WhsCode= 'CRHE'
	            GROUP BY A0.docentry) B1 ON B0.DocEntry = B1.DocEntry
			WHERE B0.CANCELED = 'N' and B0.DocStatus='O'
			GROUP BY B0.CardCode 
			) T1 ON T1.CardCode = T0.CardCode
-- OBTIENE LA EXISTENCIA Y LAS FECHAS
LEFT JOIN (
			SELECT  
				    C0.CardCode, 
					SUM(CASE WHEN C0.InvoiceDate IS NOT NULL THEN 
					        DATEDIFF(DAY,C0.InvoiceDate,GETDATE()) 
					    ELSE 
						    DATEDIFF(DAY,C0.CreateDate,GETDATE()) + 1 
						END * C0.Quantity) Total
			FROM (
					SELECT  T1.DistNumber, T0.Quantity, t1.InDate CreateDate, T1.MnfSerial CardCode,
						(SELECT max(A0.DocDate) InvoiceDate 
							FROM OINV A0 
							INNER JOIN INV1 A1 ON A0.DocEntry= A1.DocEntry    
							WHERE A0.CANCELED = 'N' and A1.U_SU_BatchAuc = T1.DistNumber and
								    A1.ItemCode = (SELECT U_Value FROM [@UG_CONFIG] WHERE Name='CU_GLO_ITEMPAYR')
								    
							) InvoiceDate
					FROM  dbo.OBTQ T0  
					INNER JOIN OBTN T1 ON T0.SysNumber=T1.SysNumber and t0.ItemCode= t1.ItemCode
					INNER JOIN OWHS T2 ON T2.WhsCode=T0.WhsCode
					INNER JOIN OLCT T3 ON T3.Code = T2.LocatiON
					INNER JOIN OCRD T5 ON T5.CardCode = T1.MnfSerial
					INNER JOIN OITM T6 ON T6.ItemCode = T0.ItemCode
					WHERE t3.LocatiON = 'CRHE' AND T0.Quantity > 0
			) C0 GROUP BY C0.CardCode
	) T2 ON T2.CardCode = T0.CardCode
-- OBTIENE EL PRECIO POR EL ARTICULO DE COBRO DE PISO
LEFT JOIN (
	SELECT A1.ItemCode, A1.Price FROM OPLN A0
	INNER JOIN ITM1 A1 ON A0.ListNum = A1.PriceList
	WHERE --A0.U_GLO_WhsCode='CRHE' AND
	 A0.U_GLO_Location = 'CRHE' AND
	 a1.ItemCode = (SELECT U_Value FROM [@UG_CONFIG] WHERE Name='CU_GLO_ITEMPAYR')
) T3 ON 1=1
-- OBTIENE EL EL IMPUESTO APLICADO AL ARTICULO DE COBRO DE PISO
LEFT JOIN (
	SELECT A1.Rate FROM OTCX A0
	INNER JOIN OSTC A1 ON A0.LnTaxCode = A1.Code
	WHERE BUSAREA = 0 AND StrVal1 = (SELECT U_Value FROM [@UG_CONFIG] WHERE Name='CU_GLO_ITEMPAYR')
) T4 ON 1=1
WHERE T2.CardCode IS NOT NULL OR T1.CardCode IS NOT NULL