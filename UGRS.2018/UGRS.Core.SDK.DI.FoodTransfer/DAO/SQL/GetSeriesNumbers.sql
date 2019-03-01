SELECT 
	T1.ItemCode                    [ItemCode],
    T1.DistNumber                  [Number],
	T2.Quantity                    [Quantity],
	T1.SysNumber                   [SysNumber]
FROM OSRI T0
	INNER JOIN (SELECT ItemCode, DistNumber, SysNumber FROM OSRN) T1 ON T1.ItemCode=T0.ItemCode AND T1.DistNumber=T0.IntrSerial
	INNER JOIN (SELECT ItemCode, SysNumber, Quantity  FROM OSRQ) T2 ON T2.ItemCode=T1.ItemCode AND T2.SysNumber=T1.SysNumber  AND T2.Quantity > 0
	LEFT JOIN (SELECT ItemCode, BaseEntry, BaseType, Direction, SysSerial  FROM SRI1) T3 ON T3.ItemCode=T0.ItemCode AND T3.SysSerial=T1.SysNumber
WHERE 
	T3.BaseEntry = '{DocEntry}' AND T3.BaseType = '{BaseType}' AND T3.Direction = 1 