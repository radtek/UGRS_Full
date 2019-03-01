SELECT  
    T6.ItemCode                    [ItemCode], 
	SUM(T0.Quantity)               [Quantity],
	T1.DistNumber                  [Batch],
	T2.WhsCode                     [Corral],
	ISNULL(CONVERT(VARCHAR, T1.ExpDate, 103), '') [AuctDate]
FROM  
	dbo.OBTQ T0  
	INNER JOIN OBTN T1 ON T0.SysNumber = T1.SysNumber AND t0.ItemCode= t1.ItemCode
	INNER JOIN OWHS T2 ON T2.WhsCode=T0.WhsCode
	INNER JOIN OLCT T3 ON T3.Code = T2.Location
	INNER JOIN OCRD T5 ON T5.CardCode = T1.MnfSerial
	INNER JOIN OITM T6 ON T6.ItemCode = T0.ItemCode
WHERE
	T3.Location = '{Location}' and T0.Quantity > 0 AND T1.MnfSerial = '{CardCode}'
GROUP BY
	T1.DistNumber,
	T1.CreateDate,
	T2.WhsCode,
	T1.ExpDate,
	T6.ItemCode
ORDER BY 
	T1.CreateDate


