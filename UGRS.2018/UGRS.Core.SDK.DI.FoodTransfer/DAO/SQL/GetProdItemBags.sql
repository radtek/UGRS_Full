SELECT 
	ISNULL(T2.U_GLO_BagsBales, 0) 
 FROM 
	(SELECT DocEntry, U_GLO_DocNum FROM OWOR) T0
	INNER JOIN (SELECT DocNum, DocEntry FROM OIGN) T1 ON T1.DocNum = T0.U_GLO_DocNum
	INNER JOIN (SELECT DocEntry, U_GLO_BagsBales FROM IGN1) T2 ON T2.DocEntry = T1.DocEntry
WHERE T0.DocEntry = '{DocEntry}'
