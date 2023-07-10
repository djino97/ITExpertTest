-- Task 3
SELECT 
  Id, Sd, Ed
FROM (
	SELECT DISTINCT
	  Id, 
	  Dt AS Sd,  
	  LEAD(dt) OVER (
		PARTITION BY id 
		ORDER BY Id, Dt) AS Ed
	FROM
	  Dates
	ORDER BY
	  Id, Dt
) AS d
WHERE
  ed is not null;