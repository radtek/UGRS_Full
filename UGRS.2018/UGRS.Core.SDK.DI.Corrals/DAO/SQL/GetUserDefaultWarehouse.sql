SELECT 
	T1.Warehouse 
FROM 
	(SELECT DfltsGroup, USER_CODE FROM OUSR) T0
	INNER JOIN (SELECT Code, Warehouse FROM OUDG) T1 on T1.Code= T0.DfltsGroup
WHERE 
	T0.USER_CODE = '{UserCode}'

