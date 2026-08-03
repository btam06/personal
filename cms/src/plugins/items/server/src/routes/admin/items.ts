const items = [
  {
      method: 'GET',
      path: '/list',
      handler: 'items.getAll',
      config: { policies: ["plugin::items.debug-auth"] }
  },
  {
      method: 'GET',
      path: '/view/:id',
      handler: 'items.getById',
      config: { policies: [] }
  },
  {
      method: 'POST',
      path: '/view',
      handler: 'items.create',
      config: { policies: [] }
  },
  {
      method: 'PUT',
      path: '/view/:id',
      handler: 'items.update',
      config: { policies: [] }
  },
  {
      method: 'DELETE',
      path: '/delete/:id',
      handler: 'items.delete',
      config: { policies: [] }
  },
];

export default items;
