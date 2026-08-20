import type { TPage } from "./page"

export type TNavItem = {
    id: number,
    title: string,
    type: string,
    path: string,
    externalPath: string,
    uiRouterKey: string,
    menuAttached: boolean,
    parent: number | null,
    master: number,
    created_at: string,
    updated_at: string,
    related: TPage
}
