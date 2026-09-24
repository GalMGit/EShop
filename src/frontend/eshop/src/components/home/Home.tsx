import { useEffect, useState } from "react";
import type { CategoryResponse } from "../../models/categories/responses/CategoryResponse.ts";
import { categoryService } from "../../services/category-service/categoryService.ts";
import { CategoryList } from "../categories/category-list/CategoryList.tsx";
import "./Home.css";

export const Home = () => {
    const [categories, setCategories] = useState<CategoryResponse[]>([]);

    useEffect(() => {
        const getCategories = async () => {
            const response = await categoryService.getAll();
            setCategories(response.data);
        };

        getCategories();
    }, []);

    return (
        <div className="home">
            <header className="home-header">
                <div>
                    <span className="home-eyebrow">
                        CATALOG
                    </span>

                    <h1>Categories</h1>

                    <p>
                        Choose a category to explore products
                    </p>
                </div>

                <div className="home-count">
                    {categories.length} categories
                </div>
            </header>

            <CategoryList categories={categories} />
        </div>
    );
};