SELECT 
    T0.CardCode              [Code],
	T0.DocEntry              [Folio],
	T0.Filler                [FromWhs],
	T0.ToWhsCode             [ToWhs],
	T1.LineNum               [LineNum],
	T1.FromWhsCod            [IFromWhs],
	T1.WhsCode               [IToWhs],
	T1.ItemCode              [Item],
	T1.Dscription            [Desc],
	T1.Quantity              [Quantity]
	
FROM 
	OWTQ T0
	INNER JOIN WTQ1 T1 ON T0.DocEntry = T1.DocEntry
WHERE 
	T0.DocNum = '{DocNum}'
