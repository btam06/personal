import type { Core } from '@strapi/strapi';

const config = ({ env }: Core.Config.Shared.ConfigParams): Core.Config.Plugin => ({
    items: {
        enabled: true,
        resolve: './src/plugins/items'
    }
});

export default config;
