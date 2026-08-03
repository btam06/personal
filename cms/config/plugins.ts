import type { Core } from '@strapi/strapi';

const config = ({ env }: Core.Config.Shared.ConfigParams): Core.Config.Plugin => ({
    items: {
        enabled: true,
        resolve: './src/plugins/items'
    },
    navigation: {
        enabled: true,
        config: {
            contentTypes: ['api::page.page'],
            contentTypesNameFields: {
                page: ['Title'],
            }
        }
    }
});

export default config;
