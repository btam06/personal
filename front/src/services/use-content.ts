import { useQuery } from "@tanstack/react-query";
import type { TNavItem } from "../types/nav-item";
import type { TPage } from "../types/page";

interface UseContentResponse {
    page: TPage | null,
    isLoading: boolean,
    isError: boolean
};

const useContent = function(path: string, navigation: string = 'main-navigation'): UseContentResponse
{
    const { data, isLoading, isError } = useQuery<Array<TNavItem>>({
        queryKey: [navigation],
        queryFn: async () => {
            const response = await fetch(`${import.meta.env.VITE_STRAPI_URL}/api/navigation/render/${navigation}`, {
                headers: {
                    Authorization: `Bearer ${import.meta.env.VITE_STRAPI_API_TOKEN}`
                }
            });

            if (response.status == 403) {
                throw new Error(`Request failed with status ${response.status}`);
            }

            return response.json();
        }
    });

    if (isError || !data) {
        return { page: null, isLoading, isError }
    }

    console.log(data);

    const navItem = data.find((item) => {
        return item.path === path;
    });

    const page = navItem?.related ?? null;

    return { page, isLoading, isError };
}

export default useContent;
