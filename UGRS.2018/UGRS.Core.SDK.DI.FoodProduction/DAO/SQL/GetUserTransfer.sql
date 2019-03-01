	 select USER_CODE from OWTQ T0 
			 inner join OUSR T1 on UserID = T0.userSign
			 where DocEntry = '{DocNum}'