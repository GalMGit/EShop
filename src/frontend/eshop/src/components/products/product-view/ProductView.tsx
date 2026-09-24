import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import "./ProductView.css";
import {productService} from "../../../services/product-service/productService.ts";
import type {ProductDetailsResponse} from "../../../models/products/responses/ProductDetailsResponse.ts";

export const ProductView = () => {
    const { productId } = useParams();
    const navigate = useNavigate();

    const [product, setProduct] = useState<ProductDetailsResponse | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (!productId) {
            return;
        }

        const loadProduct = async () => {
            try {
                setLoading(true);
                setError(null);

                const response = await productService.getById(productId);

                setProduct(response.data);
            } catch (error) {
                console.error(error);
                setError("Failed to load product");
            } finally {
                setLoading(false);
            }
        };

        loadProduct();
    }, [productId]);

    if (loading) {
        return (
            <main className="product-view">
                <div className="product-view-status">
                    Loading...
                </div>
            </main>
        );
    }

    if (error || !product) {
        return (
            <main className="product-view">
                <div className="product-view-status">
                    <p>{error ?? "Product not found"}</p>

                    <button onClick={() => navigate(-1)}>
                        Go back
                    </button>
                </div>
            </main>
        );
    }

    return (
        <main className="product-view">
            <button
                className="product-back-button"
                onClick={() => navigate(-1)}
            >
                ← Back
            </button>

            <section className="product-details">
                <div className="product-details-image">
                    {product.mediaUrl ? (
                        <img
                            src={product.mediaUrl}
                            alt={product.name}
                        />
                    ) : (
                        <div className="product-details-placeholder">
                            No image
                        </div>
                    )}
                </div>

                <div className="product-details-content">
                    <span className="product-details-label">
                        PRODUCT
                    </span>
                    <h4 className="product-details-stock">
                        {product.brand}
                    </h4>

                    <h1>{product.name}</h1>

                    <div className="product-details-price">
                        {product.price.toFixed(2)}Р
                    </div>

                    <p className="product-details-description">
                        {product.description}
                    </p>

                    <div className="product-details-stock">
                        {product.availableQuantity > 0
                            ? `${product.availableQuantity} available`
                            : "Out of stock"}
                    </div>

                    {Object.keys(product.specifications).length > 0 && (
                        <div className="product-specifications">
                            <h2>Specifications</h2>

                            <div className="specifications-list">
                                {Object.entries(product.specifications).map(
                                    ([key, value]) => (
                                        <div
                                            className="specification-row"
                                            key={key}
                                        >
                                            <span>{key}</span>
                                            <strong>
                                                {String(value)}
                                            </strong>
                                        </div>
                                    )
                                )}
                            </div>
                        </div>
                    )}
                </div>
            </section>
        </main>
    );
};