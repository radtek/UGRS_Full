SELECT 
	WipAcct 
FROM 
	OGAR T0 
	LEFT JOIN (SELECT Code FROM OSTC) T1 on T1.Code = T0.VatGroup
	LEFT JOIN (SELECT ItmsGrpCod, ItemCode FROM OITM) T2 on T2.ItmsGrpCod = T0.ItmsGrpCod
WHERE 
	T2.ItemCode= '{ItemCode}' AND T0.WhsCode= '{Whs}' AND T1.Code IS NULL AND  T0.[Year] = YEAR(GETDATE())