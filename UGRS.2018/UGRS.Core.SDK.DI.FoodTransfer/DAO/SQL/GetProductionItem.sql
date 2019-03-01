SELECT 
    T0.LineNum                [LineNum],
	T0.ItemCode               [Item],
	T1.ItemName               [Desc], 
	T0.WhsCode                [Whs],
	T1.OnHand                 [Exist], 
	T0.Quantity               [Plan], 
	T0.Quantity               [Qty],
	U_GLO_BagsBales           [Bags],
	0                         [Consumed],
	ISNULL(T2.WipAcct, '')    [AccCode],
    ''                        [Method],
	CASE WHEN T1.QryGroup27 = 'Y' THEN 1 ELSE 0 END [Prod],
	CASE WHEN T1.QryGroup24 = 'Y' THEN 1 ELSE 0 END [BagsRequired],
	0                                               [Resource],
	0                                               [Inventorial]
FROM 
	IGN1 T0
	INNER JOIN (SELECT ItemCode, QryGroup27, QryGroup24, ItemName, OnHand FROM OITM) T1 ON T1.ItemCode = T0.ItemCode
	LEFT JOIN (SELECT TOP 1 
						A0.WipAcct,
						A0.WhsCode
				FROM 
					OGAR A0 
				WHERE
					A0."Year" = YEAR(GETDATE()) AND ISNULL(A0.WipAcct,'') <> '') T2 ON T2.WhsCode = T0.WhsCode 
WHERE 
	T0.DocEntry = '{DocEntry}'
