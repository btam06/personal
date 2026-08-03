// src/api/items-proxy/services/api-client.ts
import type { Core } from '@strapi/strapi';

/**
 * Token cache interface
 */
interface TokenCache {
    value: string;
    expiresAt: number;
}

/**
 * Token cache storage
 */
let cachedToken: TokenCache | null = null;

/**
 * Get access token from the api
 * @returns Promise<string>
 */
async function getAccessToken(): Promise<string> {
    if (cachedToken && Date.now() < cachedToken.expiresAt) {
        return cachedToken.value;
    }

    const apiUrl       = process.env.DOTNET_API_URL;
    const clientId     = process.env.DOTNET_CLIENT_ID ?? 'strapi-cms';
    const clientSecret = process.env.DOTNET_CLIENT_SECRET;

    if (!apiUrl || !clientSecret) {
        throw new Error('DOTNET_API_URL or DOTNET_CLIENT_SECRET is not configured.');
    }

    const response = await fetch(`${apiUrl}/connect/token`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        body: new URLSearchParams({
            grant_type: 'client_credentials',
            client_id: clientId,
            client_secret: clientSecret,
            scope: 'items.write',
        }),
    });

    if (!response.ok) {
        const body = await response.text();
        throw new Error(`Failed to fetch access token: ${response.status} ${body}`);
    }

    const data = await response.json();
    cachedToken = {
        value: data['access_token'],
        expiresAt: Date.now() + (data['expires_in'] - 30) * 1000, // refresh 30s early
    };

    return cachedToken.value;
}

/**
 * Request from the api
 */
export default ({ strapi }: { strapi: Core.Strapi }) => ({
    async request(path: string, options: RequestInit = {}) {
        const token  = await getAccessToken();
        const apiUrl = process.env.DOTNET_API_URL;

        const response = await fetch(`${apiUrl}${path}`, {
            ...options,
            headers: {
                ...options.headers,
                Authorization: `Bearer ${token}`,
                'Content-Type': 'application/json',
            },
        });

        return response;
    },
});
