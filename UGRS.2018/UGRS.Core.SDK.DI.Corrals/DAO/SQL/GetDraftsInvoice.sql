SELECT DocEntry, DocNum, DocStatus, U_GLO_Status,
DocDate, CardCode, CardName, DocCur, NumAtCard,
CASE WHEN DocCur = 'MXP' then DocTotal else DocTotalFC end AS Total,
Comments, U_PE_Origin, ObjType
FROM ODRF
WHERE U_PE_Origin = '{Type}'
AND NumAtCard = 'CM_CR_{Date}'
AND U_GLO_Status != 'C' 
AND CANCELED = 'N' 
AND ObjType = 13 
AND DocStatus = 'O'