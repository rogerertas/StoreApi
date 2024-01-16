import React, { useState, useEffect } from 'react';
import { fetchProducts } from '../services/api';

function ProductList() {
  const [products, setProducts] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 8;

  useEffect(() => {
    const getProducts = async () => {
      try {
        const data = await fetchProducts(currentPage, pageSize);
        setProducts(data);
      } catch (error) {
        console.error("Error fetching products:", error);
      }
    };

    getProducts();
  }, [currentPage]);

  return (
    <div>
      <h2>Products</h2>
      <ul>
        {products.map(product => (
          <li key={product.id}>{product.title}</li>
        ))}
      </ul>
      <button onClick={() => setCurrentPage(currentPage - 1)} disabled={currentPage === 1}>Previous</button>
      <button onClick={() => setCurrentPage(currentPage + 1)}>Next</button>
    </div>
  );
}

export default ProductList;
