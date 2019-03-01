  SELECT   
    Q0.WhsCode               [Corral],  
	Q0.Quantity              [Existence],
	Q0.DistNumber            [Batch],
				SUM(CASE WHEN Q0.InvoiceDate IS NOT NULL THEN 
					DATEDIFF(DAY,Q0.InvoiceDate,Q0.DateTrans) 
				ELSE 
					DATEDIFF(DAY,Q0.CreateLote,Q0.DateTrans) +1  
				END)   TotalDays,
					q0.DocEntry
			FROM 
				(SELECT 
				   W0.BatchNum, W0.quantity,W0.DateTrans,W0.CreateLote,W0.cardcode,w0.docentry, w0.WhsCode, W0.DistNumber,
				   (SELECT MAX(R0.DocDate) InvoiceDate 
										FROM OINV R0 
										INNER JOIN INV1 R1 ON R0.DocEntry= R1.DocEntry    
										WHERE R0.CANCELED = 'N' AND R1.U_SU_BatchAuc = W0.BatchNum AND
												R1.ItemCode = (SELECT U_Value FROM [@UG_CONFIG] WHERE "Name" = 'CU_GLO_ITEMPAYR') AND R1.DocDate < W0.DateTrans AND U_PE_Origin = 'S'
					) InvoiceDate
				FROM
					(SELECT 
						SUM(P5.Quantity) Quantity, P5.BatchNum, P0.DocEntry, P5.DocDate DateTrans, P6.inDate CreateLote, P6.MnfSerial CardCode, P2.WhsCode, P6.DistNumber
					FROM OWTR P0
					INNER JOIN WTR1 P1 ON P0.Docentry= P1.Docentry
					INNER JOIN OWHS P2 ON P2.WhsCode = P1.FromWhsCod
					INNER JOIN OLCT P4 ON P4.Code = P2.Location AND P4.Location ='{Location}'
					INNER JOIN OITM T4 ON P1.ItemCode = T4.ItemCode AND t4.ItmsGrpCod = (select u_value from [@UG_CONFIG] where name ='GLO_ITEMGANADO')
					INNER JOIN IBT1 P5 ON P5.BaseLinNum= P1.LineNum AND P5.BaseEntry = P1.DocEntry AND P5.BaseType = 67  AND P5.WhsCode ='SUHE'
					INNER JOIN OBTN P6 ON P6.DistNumber= P5.BatchNum AND P6.ItemCode = P1.ItemCode
					WHERE P0.U_GLO_STATUS <> 'C' AND P6.MnfSerial = '{CardCode}'
					GROUP BY  P5.BatchNum, P5.DocDate,P6.inDate,P0.DocEntry, p6.MnfSerial,  P2.WhsCode, P6.DistNumber) W0
				) Q0
				GROUP BY q0.DocEntry, Q0.WhsCode, Q0.Quantity, Q0.DistNumber