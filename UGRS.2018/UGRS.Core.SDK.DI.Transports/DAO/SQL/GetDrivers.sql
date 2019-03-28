--SELECT (lastName+' '+firstName) as Name,sex,type FROM OHEM 
--WHERE 
--CostCenter = '{CostingCode}'
--AND (lastName+' '+firstName) = COALESCE(NULLIF('{Name}',''),(lastName+' '+firstName)) 
--AND pager = 'N'
--AND (lastName+' '+firstName like '%{Search}%' or sex like '%{Search}%' or type like '%{Search}%')

SELECT (t0.lastName+' '+t0.firstName) as Name,t0.sex,T0.empID as Type FROM OHEM T0
inner join OHPS T1 on t0.position = t1.posID
WHERE 
T0.CostCenter = '{CostingCode}'
AND (t0.lastName+' '+t0.firstName) = COALESCE(NULLIF('{Name}',''),(t0.lastName+' '+t0.firstName)) 
AND t0.pager = 'No' AND t0.POSITION='3'
AND (lastName+' '+firstName like '%{Search}%' or sex like '%{Search}%' or type like '%{Search}%')