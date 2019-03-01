SELECT
	-(ISNULL(T0.TransValue, 0)+ ISNULL(T1.TransValue, 0) + ISNULL(T2.TransValue, 0))
FROM 
	OINM T0
	LEFT JOIN OINM T1 ON T1.ItemCode = T0.ItemCode AND T1.TransType = 202 AND T1.CreatedBy = '{OrderID}'
	LEFT JOIN OINM T2 ON T2.ItemCode = T0.ItemCode AND T2.TransType = 59 AND T2.CreatedBy = 
	CASE WHEN (SELECT COUNT(DocEntry) FROM OIGN WHERE DocNum = (SELECT U_GLO_DocNum FROM OWOR WHERE DocEntry = '{OrderID}')) = 0 
	THEN '{OrderID}'
	ELSE  (SELECT DocEntry FROM OIGN WHERE DocNum = (SELECT U_GLO_DocNum FROM OWOR WHERE DocEntry = '{OrderID}')) END
WHERE 
	T0.CreatedBy = '{ExitID}' AND T0.TransType = 60