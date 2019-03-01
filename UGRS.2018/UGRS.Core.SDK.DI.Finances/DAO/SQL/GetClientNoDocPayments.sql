SELECT DocEntry, DocNum, Series, CardCode, DocDate, DocDueDate, DocTotal, DocTotalFC, TransId, 
DocCurr as DocCur, NoDocSum, NoDocSumFC, PayNoDoc, OpenBal, OpenBalFc
FROM ORCT
WHERE PayNoDoc = 'Y' AND CardCode = '{CardCode}' AND OpenBal > 0