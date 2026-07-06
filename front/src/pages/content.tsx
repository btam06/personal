
import { useQuery } from "@tanstack/react-query"
import { useParams } from "react-router"
import NotFound from "./not-found"
import type { TLanding } from "../types/landing"
import { BlocksRenderer } from "@strapi/blocks-react-renderer"

export default function Content() {
    const { page } = useParams()
    const { data, isLoading, isError } = useQuery<TLanding>({
        queryKey: [page],
        queryFn: async () => {
            const response = await fetch(`${import.meta.env.VITE_STRAPI_URL}/api/${page}`)
            return response.json()
        }
    })

    if (isError) return NotFound

    if (isLoading) return (
        <div>Loading...</div>
    )

    return (
        <section className="main">
            <header>{data.data.Title}</header>
            <div className="content">
                <BlocksRenderer content={data.data.Body} />
            </div>
        </section>
    )
}
