import { useNavigate } from "react-router-dom";
import type { ProductListItemResponse } from "../../../models/products/responses/ProductListItemResponse.ts";
import "./ProductCard.css";

interface ProductCardProps {
    product: ProductListItemResponse;
}

export const ProductCard = ({ product }: ProductCardProps) => {
    const navigate = useNavigate();

    const handleClick = () => {
        navigate(`/products/${product.id}`);
    };

    return (
        <article
            className="product-card"
            onClick={handleClick}
        >
            <div className="product-image-wrapper">
                {product.thumbnailUrl ? (
                    <img
                        className="product-image"
                        src={product.thumbnailUrl}
                        alt={product.name}
                    />
                ) : (
                    <div className="product-image-placeholder">
                        No image
                    </div>
                )}
            </div>

            <div className="product-info">
                <span className="product-label">
                    PRODUCT
                </span>
                <h4>{product.price}Р</h4>
                <h3>
                    {product.name}
                </h3>
            </div>
        </article>
    );
};