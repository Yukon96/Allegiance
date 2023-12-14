-- Declare the variables
DECLARE @LetterX VARCHAR(1) = 'A';
DECLARE @LetterY VARCHAR(1) = 'Y';

-- Calculate the distance and rank for each cell
WITH DistanceCalculation AS (
    SELECT
        c.*,
        ABS(c.x_cord - s.x_cord) + ABS(c.y_cord - s.y_cord) AS distance,
        ROW_NUMBER() OVER (PARTITION BY c.color ORDER BY ABS(c.x_cord - s.x_cord) + ABS(c.y_cord - s.y_cord)) AS rnk --Dunno asked a freind of mine who is a dba
    FROM
        cells c
    CROSS JOIN
        (SELECT x_cord, y_cord FROM cells WHERE letter_x = @LetterX AND letter_y = @LetterY) s
    WHERE
        c.color IN ('blue', 'green', 'yellow')
)

-- Select the closest blue, green, and yellow cells
SELECT cell_id, x_cord, y_cord, letter_x, letter_y, color, acid
FROM DistanceCalculation
WHERE rnk = 1;
