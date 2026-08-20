import { useLocation } from "react-router"
import NotFound from "./not-found"
import { BlocksRenderer } from "@strapi/blocks-react-renderer"
import useContent from "../services/use-content"
import Loading from "../partials/loading"

export default function ViewPage() {
    const location                     = useLocation();
    const { page, isLoading, isError } = useContent(location.pathname);

    if (isError) return <NotFound />
    if (isLoading) return <Loading />

    return (
        <section className="main">
            <header>{page.Title}</header>
            <div className="content">
                <BlocksRenderer content={page.Body} />
            </div>
        </section>
    )
}
