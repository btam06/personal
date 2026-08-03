import type { Core } from "@strapi/strapi";

const items = ({ strapi }: { strapi: Core.Strapi }) => ({

  async getAll(ctx) {
    const client   = strapi.plugin('items').service('api-client');
      const response = await client.request('/api/items');
      ctx.status = response.status;
      ctx.body   = await response.json();
  },

  async getById(ctx) {
      const { id }   = ctx.params;
      const client   = strapi.plugin('items').service('api-client');
      const response = await client.request(`/api/items/${id}`);
      ctx.status = response.status;
      ctx.body   = response.status === 404 ? undefined : await response.json();
  },

  async create(ctx) {
      const client   = strapi.plugin('items').service('api-client');
      const response = await client.request('/api/items', {
          method: 'POST',
          body: JSON.stringify(ctx.request.body),
      });
      ctx.status = response.status;
      ctx.body   = await response.json();
  },

  async update(ctx) {
      const { id }   = ctx.params;
      const client   = strapi.plugin('items').service('api-client');
      const response = await client.request(`/api/items/${id}`, {
          method: 'PUT',
          body: JSON.stringify(ctx.request.body),
      });
      ctx.status = response.status;
      ctx.body   = response.status === 404 ? undefined : await response.json();
  },

  async delete(ctx) {
      const { id }   = ctx.params;
      const client   = strapi.plugin('items').service('api-client');
      const response = await client.request(`/api/items/${id}`, {
          method: 'DELETE',
      });
      ctx.status = response.status;
  },
});

export default items;
