SELECT 
	DISTINCT T2.ItemCode, CONVERT(DECIMAL(10,2),T4.Price) [Price]
FROM 
	(SELECT U_NAME, DfltsGroup FROM dbo.OUSR) T0
	INNER JOIN (SELECT Warehouse, Code FROM OUDG) T1 ON T1.Code= T0.DfltsGroup
	INNER JOIN (SELECT ItemCode, WhsCode, Locked FROM OITW) T2 ON T2.WhsCode = T1.Warehouse AND T2.Locked = 'N'
	INNER JOIN (SELECT ItemCode, SellItem, validFor, ItemType FROM OITM) T3 ON T2.ItemCode = T3.ItemCode AND T3.SellItem = 'Y'
	INNER JOIN (SELECT Price, ItemCode, PriceList FROM ITM1) T4 ON T4.ItemCode = T2.ItemCode AND T3.validFor='Y' and T3.itemType ='I'
WHERE 
	T0.U_NAME = '{UserName}' AND T4.PriceList = (SELECT ListNum FROM OPLN WHERE U_GLO_Location = T2.WhsCode)