select U_Folio from [@UG_SU_AUTN] where U_Opened = 'N' and 
((U_AutCorral='N' and 'CR_CORRA' ='{CenterCost}') or (U_AutTransp='N' and 'TR_TRANS' ='{CenterCost}') or 
((select 1 from OUSR T0
inner join OHEM T1 on T0.USERID = T1.userId
inner join HEM6 T2 on T2.empID = T1.empID
inner join OHTY T3 on T3.typeID = T2.roleID
where T3.descriptio Like 'CYCSU' and T0.USER_CODE = '{UserID}') = 1 and U_AutFz='N'and U_AutCyC = 'N' and U_AutAuction ='Y' and U_AutCorral='Y' and  U_AutTransp='Y'))
