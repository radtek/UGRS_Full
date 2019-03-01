SELECT  
  T6.ItemCode                                   [ItemCode],
  T6.ItemName                                   [ItemName],
  T0.WhsCode                                    [Corral],
  ISNULL(CONVERT(VARCHAR, T1.ExpDate, 103), '') [AuctDate],
  SUM(T0.Quantity)                              [Exist],
  0                                             [Quantity],
  T6.U_CR_Value                                 [Import]
FROM  dbo.OBTQ T0  
	INNER JOIN OBTN T1 ON T0.SysNumber=T1.SysNumber and t0.ItemCode= t1.ItemCode
	INNER JOIN OWHS T2 ON T2.WhsCode=T0.WhsCode
	INNER JOIN OLCT T3 ON T3.Code = T2.Location
	INNER JOIN OCRD T5 ON T5.CardCode = T1.MnfSerial
	INNER JOIN OITM T6 ON T6.ItemCode = T0.ItemCode
WHERE 
	t3.Location = '{Location}' AND T0.Quantity > 0 AND T5.CardCode = '{CardCode}'
GROUP BY 
	T6.ItemCode,T6.ItemName,T0.WhsCode,T1.ExpDate, T6.U_CR_Value
