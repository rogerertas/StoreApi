import axios from 'axios';

const API_BASE_URL = 'https://localhost:7286';

export const fetchProducts = async (page = 1, pageSize = 8, minPrice = null, maxPrice = null, category = null) => {
  const queryParams = new URLSearchParams({ page, pageSize });
  if (minPrice !== null) queryParams.append('minPrice', minPrice);
  if (maxPrice !== null) queryParams.append('maxPrice', maxPrice);
  if (category !== null) queryParams.append('category', category);

  try {
    const response = await axios.get(`${API_BASE_URL}/products?${queryParams}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching products:', error);
    return [];
  }
};
