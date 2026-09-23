import type { ProductListItemResponse } from "../../../models/products/responses/ProductListItemResponse.ts";
import { ProductCard } from "../product-card/ProductCard.tsx";

interface ProductListProps {
    products: ProductListItemResponse[];
}

export const ProductList = ({ products }: ProductListProps) => {
    return (
        <section className="product-grid">
            {products.map((product) => (
                <ProductCard
                    product={product}
                    key={product.id}
                />
            ))}
        </section>
    );
};