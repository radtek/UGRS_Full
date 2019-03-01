SELECT T1.firstName, T2.descriptio, T3.USER_CODE
FROM HEM6 T0
INNER JOIN OHEM T1 ON T0.empID = T1.empID /* Employees */
INNER JOIN OHTY T2 ON T0.roleID = T2.typeID /* Roles */
INNER JOIN OUSR T3 ON T1.userId = T3.USERID /* Users */
WHERE T3.userId = '{UserId}' AND T2.descriptio LIKE '{RoleName}'