import type { CategoryResponse } from "../../../models/categories/responses/CategoryResponse.ts";
import { useNavigate } from "react-router-dom";
import "./CategoryCard.css";

interface CategoryCardProps {
    category: CategoryResponse;
}

export const CategoryCard = ({ category }: CategoryCardProps) => {
    const navigate = useNavigate();

    return (
        <article
            className="category-card"
            onClick={() => navigate(`/categories/${category.id}`)}
        >
            <div className="category-card-content">
                <span className="category-label">
                    CATEGORY
                </span>

                <h2>
                    {category.name}
                </h2>
            </div>

            <div className="category-arrow">
                →
            </div>
        </article>
    );
};