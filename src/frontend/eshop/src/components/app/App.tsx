import './App.css';
import { ProductList } from "../products/product-list/ProductList.tsx";
import { useEffect, useState } from "react";
import { productService } from "../../services/product-service/productService.ts";
import type { ProductListItemResponse } from "../../models/products/responses/ProductListItemResponse.ts";

export const App = () => {
  const [products, setProducts] = useState<ProductListItemResponse[]>([]);

  useEffect(() => {
    const getProducts = async () => {
      const response = await productService.getAll();
      setProducts(response.data);
    };

    getProducts();
  }, []);

  return (
      <main className="app">
        <header className="app-header">
          <div>
            <span className="eyebrow">CATALOG</span>
            <h1>Products</h1>
            <p>Explore our collection</p>
          </div>

          <div className="product-count">
            {products.length} products
          </div>
        </header>

        <ProductList products={products} />
      </main>
  );
};

