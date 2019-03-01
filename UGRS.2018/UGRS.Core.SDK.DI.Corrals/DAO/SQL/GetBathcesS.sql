SELECT  
	SUM(P5.Quantity)         [Quantity],
	P6.DistNumber            [Batch],
	P2.WhsCode               [Corral]	
FROM 
	OWTR P0
	INNER JOIN WTR1 P1 ON P0.Docentry= P1.Docentry
	INNER JOIN OWHS P2 ON P2.WhsCode = P1.FromWhsCod
	INNER JOIN OLCT P4 ON P4.Code = P2.Location AND P4.Location ='{Location}'
	INNER JOIN OITM T4 ON P1.ItemCode = T4.ItemCode AND t4.ItmsGrpCod = 182
	INNER JOIN IBT1 P5 ON P5.BaseLinNum= P1.LineNum AND P5.BaseEntry = P1.DocEntry AND P5.BaseType = 67  AND P5.WhsCode ='SUHE'
	INNER JOIN OBTN P6 ON P6.DistNumber= P5.BatchNum AND P6.ItemCode = P1.ItemCode
WHERE 
	P0.U_GLO_Status <> 'C' AND p6.MnfSerial = '{CardCode}'
GROUP BY
	P6.DistNumber,
	p6.CreateDate,
	P2.WhsCode
ORDER BY 
	P6.CreateDate
DESC