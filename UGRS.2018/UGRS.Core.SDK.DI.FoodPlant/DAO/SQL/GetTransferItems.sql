SELECT 
	ItemCode                [Item], 
	Dscription              [Desc], 
	Quantity                [Quantity], 
	U_GLO_BagsBales         [Bags],
	FromWhsCod              [FromWhs],
	WhsCode                 [ToWhs]
FROM 
	WTR1 
WHERE 
	DocEntry = '{DocEntry}'
