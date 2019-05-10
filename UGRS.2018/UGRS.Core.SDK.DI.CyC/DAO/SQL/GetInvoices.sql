select T0.DocEntry,
Max(T0.DocNum) DocNum, 
Max(T0.DocDate) DocDate,
DATEDIFF(day, Max(T0.DocDueDate), 
GetDate()) as Days, Max(T1.OcrCode) OcrCode, 
(CASE WHEN t0.DocCur = 'USD' THEN 
(MAX(t0.DocTotalFC) * (SELECT Rate FROM ORTT WHERE CONVERT(DATE, GETDATE()) = CONVERT(DATE,RateDate))) 
ELSE MAX(t0.DocTotal) END - MAX(t0.PaidToDate)) as Balance
from OINV T0 
inner join INV1 T1 on T1.DocEntry = T0.DocEntry
where   T0.CardCode = '{CardCode}' and DocStatus = 'O' and T0.CANCELED = 'N' and 
(T1.OcrCode = '{OcrCode}' or '{CYC}' = 'Y') 
group by T0.DocEntry,t0.DocCur
