SELECT CheckKey, CheckNum, BankCode, Branch, CashCheck, CardCode, RcptNum, CheckSum, Deposited, Converted,
BankAcct, AcctNum, Currency, Canceled, CardName, CheckDate
FROM OCHH
WHERE RcptNum = '{DocEntry}' AND AcctNum = '{BankAcct}' AND CheckNum = '{CheckNum}' AND CheckSum = '{CheckSum}'