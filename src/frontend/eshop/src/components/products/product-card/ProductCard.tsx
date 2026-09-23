import type { ProductListItemResponse } from "../../../models/products/responses/ProductListItemResponse.ts";

interface ProductCardProps {
    product: ProductListItemResponse;
}

export const ProductCard = ({ product }: ProductCardProps) => {
    return (
        <article className="product-card">
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
                <span className="product-label">PRODUCT</span>

                <h3>{product.name}</h3>
            </div>
        </article>
    );
};