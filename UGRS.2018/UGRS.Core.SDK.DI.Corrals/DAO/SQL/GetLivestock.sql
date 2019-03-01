SELECT  
  T5.CardCode                                   [Code],
  T5.CardName                                   [Name],
  T0.WhsCode                                    [Corral],
  SUM(T0.Quantity)                              [Exist],
  0                                             [Quantity],
  T6.ItemCode                                   [ItemCode],
  T6.ItemName                                   [ItemName],
  ISNULL(CONVERT(VARCHAR, T1.ExpDate, 103), '') [AuctDate]
FROM  (SELECT Quantity, SysNumber, ItemCode, WhsCode FROM dbo.OBTQ) T0  
	INNER JOIN (SELECT SysNumber, ItemCode, MnfSerial, ExpDate FROM OBTN) T1 ON T0.SysNumber = T1.SysNumber AND T0.ItemCode = T1.ItemCode
	INNER JOIN (SELECT Location, WhsCode FROM OWHS) T2 ON T2.WhsCode = T0.WhsCode
	INNER JOIN (SELECT Code, Location FROM OLCT) T3 ON T3.Code = T2.Location
	INNER JOIN (SELECT CardCode, CardName FROM OCRD) T5 ON T5.CardCode = T1.MnfSerial
	INNER JOIN (SELECT ItemCode, ItemName FROM OITM) T6 ON T6.ItemCode = T0.ItemCode
WHERE 
	T3.Location = '{Location}' AND T0.Quantity > 0 AND T5.CardCode = '{CardCode}' AND T1.ExpDate = '{AuctionDate}'
GROUP BY 
	T0.WhsCode, T6.ItemCode, T6.ItemName, T5.CardCode, T5.CardName, T1.ExpDate