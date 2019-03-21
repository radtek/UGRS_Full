SELECT T0.DocEntry, T0.DocNum, T0.DocDate, T0.U_MQ_TipoCont, T0.CardName,
CASE
    WHEN CANCELED = 'Y'
		THEN 'Cancelled'
    ELSE T0.U_GLO_Status
END AS Status,
CASE 
	WHEN T0.DocCur = 'MXP' 
		THEN T0.DocTotal 
	ELSE T0.DocTotalFC
END AS Total,
ISNULL(T0.U_GLO_Municipality, '') AS 'MunicipalityId', T1.Name AS 'Municipality'
FROM ORDR T0
INNER JOIN [@UG_GLO_TOWN] T1
	ON T1.Code = T0.U_GLO_Municipality
WHERE T0.DocEntry = '{DocEntry}'