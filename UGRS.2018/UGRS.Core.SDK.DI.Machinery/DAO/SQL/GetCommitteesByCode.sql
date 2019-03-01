SELECT T1.Code, T1.Name FROM [@UG_GR_COEE] T1
INNER JOIN [@UG_GLO_TOWN] T2
	ON T1.Code = T2.U_Commite
WHERE T2.Code = '{MunicipalityCode}' ORDER BY T1.Name