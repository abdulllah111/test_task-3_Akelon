# test_task-3_Akelon


SELECT p.id, p.name
FROM products p
INNER JOIN orders o ON p.id = o.product_id
WHERE p.unit_price IS NULL;



UPDATE products
SET unit_price = unit_price * 1.05
WHERE id IN (
    SELECT DISTINCT o.product_id
    FROM orders o
    WHERE EXTRACT(YEAR_MONTH FROM posting_date) = EXTRACT(YEAR_MONTH FROM CURDATE() - INTERVAL 1 MONTH)
);



ALTER TABLE orders ADD COLUMN order_name VARCHAR(255);

UPDATE orders o
JOIN products p ON o.product_id = p.id
SET o.order_name = CONCAT('Заявка №', o.order_number, ' на приобретение ', p.name);