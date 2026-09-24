import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import type { ProductListItemResponse } from "../../../models/products/responses/ProductListItemResponse.ts";
import { productService } from "../../../services/product-service/productService.ts";
import { ProductList } from "../../products/product-list/ProductList.tsx";
import "./CategoryView.css";

export const CategoryView = () => {
    const { categoryId } = useParams();
    const navigate = useNavigate();

    const [products, setProducts] = useState<ProductListItemResponse[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        if (!categoryId) {
            return;
        }

        const getProducts = async () => {
            try {
                setLoading(true);

                const response =
                    await productService.getByCategoryId(categoryId);

                setProducts(response.data);
            } finally {
                setLoading(false);
            }
        };

        getProducts();
    }, [categoryId]);

    return (
        <div className="category-view">
            <header className="category-view-header">

                <div>
                    <button
                        className="category-back-button"
                        onClick={() => navigate("/")}
                    >
                        ← Back to categories
                    </button>

                    <span className="category-view-eyebrow">
                        CATEGORY
                    </span>

                    <h1>
                        Products
                    </h1>

                    <p>
                        Products available in this category
                    </p>
                </div>

                <div className="category-view-count">
                    {products.length} products
                </div>

            </header>

            {loading ? (
                <div className="category-loading">
                    Loading products...
                </div>
            ) : products.length === 0 ? (
                <div className="category-empty">
                    <h2>No products</h2>

                    <p>
                        There are no products in this category yet.
                    </p>
                </div>
            ) : (
                <ProductList products={products} />
            )}
        </div>
    );
};