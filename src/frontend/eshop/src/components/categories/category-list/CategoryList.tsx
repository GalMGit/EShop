import type { CategoryResponse } from "../../../models/categories/responses/CategoryResponse.ts";
import { CategoryCard } from "../category-card/CategoryCard.tsx";
import "./CategoryList.css";

interface CategoryListProps {
    categories: CategoryResponse[];
}

export const CategoryList = ({ categories }: CategoryListProps) => {
    return (
        <section className="category-grid">
            {categories.map((category) => (
                <CategoryCard
                    category={category}
                    key={category.id}
                />
            ))}
        </section>
    );
};