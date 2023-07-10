-- Task 1, query 2
SELECT 
    ClientName, 
	(
      SELECT count(*) 
      FROM ClientContacts 
      WHERE ClientContacts.ClientId = Clients.Id
    ) AS cont_count
FROM
Clients;

-- Task 2, query 2
SELECT 
  Clients.Id,
  Clients.ClientName,
  cont.cont_count
FROM
(
  SELECT 
    ClientId, count(ClientId) as cont_count
  FROM
    ClientContacts 
  GROUP BY
    ClientId
  HAVING COUNT(ClientId) > 2
  ORDER BY
    ClientId
) as cont
INNER JOIN Clients on cont.ClientId = Clients.Id;
	
