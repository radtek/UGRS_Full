SELECT T1.ItemCode, T1.Dscription ItemName, sum(isnull(T1.Quantity, 0)) Quantity, 
CASE   
    WHEN T2.QryGroup26 = 'Y' THEN '15W40'
	WHEN T2.QryGroup28 = 'Y' THEN 'Hidraulico'
	WHEN T2.QryGroup29 = 'Y' THEN 'SAE40'
	WHEN T2.QryGroup30 = 'Y' THEN 'Transmision'
	WHEN T2.QryGroup23 = 'Y' THEN 'Grasas'
	ELSE 'Unk'
 END Category
FROM OIGE T0
INNER JOIN IGE1 T1 
	ON T0.DocEntry = T1.DocEntry
INNER JOIN OITM T2
	ON T2.ItemCode = T1.ItemCode
WHERE T0.U_MQ_Rise = '{RiseId}' AND T0.CANCELED = 'N' 
GROUP BY T1.ItemCode, T1.Dscription, T2.QryGroup26, T2.QryGroup28, T2.QryGroup29, T2.QryGroup30, T2.QryGroup23