SELECT TOP 1
	 WhsCode 
FROM 
	(SELECT WhsCode, ItemCode, StockValue FROM OITW) OITW 
WHERE 
	ItemCode = '{ItemCode}' AND WhsCode LIKE '%PLHETR%' AND StockValue = 0
