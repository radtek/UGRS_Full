SELECT 
	T0.DocEntry              [Folio],
	T0.Filler                [FromWhs],
	T0.ToWhsCode             [ToWhs],
	T1.LineNum               [LineNum],
	T1.FromWhsCod            [IFromWhs],
	T1.WhsCode               [IToWhs],
	T1.ItemCode              [Item],
	T1.Dscription            [Desc],
	T1.OpenQty               [Quantity],
	CONVERT(VARCHAR, T0.DocNum)[DocNum],
	T0.UserSign                [UserID]
	
	
FROM 
	OWTQ T0
	INNER JOIN (SELECT DocEntry, LineNum, FromWhsCod, WhsCode, ItemCode, Dscription, Quantity, OpenQty FROM WTQ1) T1 ON T0.DocEntry = T1.DocEntry
WHERE 
	T0.DocNum = '{DocNum}' AND T0.Filler = '{Whs}' AND  T0.U_GLO_Status = 'O'