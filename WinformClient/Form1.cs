using Kukil.PickingNumberService.MessageContracts;
using MassTransit;
using MassTransit.NLogIntegration.Logging;
using MassTransit.Util;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinformClient
{
	public partial class Form1 : Form
	{
		private IBusControl _busControl;
		private IRequestClient<ILoadOrderRequest, ILoadOrderResponse> client;

		public Form1()
		{
			InitializeComponent();
		}

		private async void button1_Click(object sender, EventArgs e)
		{
			try
			{
				// 대기 필요
				//Task.Run(async () =>
				//{
				//	var response = await client.Request(new LoadOrderRequest(int.Parse(textBox1.Text)));

				//	MessageBox.Show($"key: {response.DocumentKey}, generating: {response.GeneratedLoadOrderNumber}, at: {response.Timestamp}");
				//}).Wait();

				var response = await client.Request(new LoadOrderRequest(int.Parse(textBox1.Text)));
				MessageBox.Show($"key: {response.DocumentKey}, generating: {response.GeneratedLoadOrderNumber}, at: {response.Timestamp}");
			}
			catch (Exception ex)
			{
				MessageBox.Show($"{ex.Message}");
			}
		}

		private IRequestClient<ILoadOrderRequest, ILoadOrderResponse> CreateRequestClient(IBusControl _busControl)
		{
			var serviceAddress = new Uri("rabbitmq://sapdev1.semubot.com/kukil/loadOrder.request.service");
			IRequestClient<ILoadOrderRequest, ILoadOrderResponse> client = _busControl.CreateRequestClient<ILoadOrderRequest, ILoadOrderResponse>(serviceAddress, TimeSpan.FromSeconds(10));

			return client;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			NLogLogger.Use();
			_busControl = CreateBus();
			client = CreateRequestClient(_busControl);

			TaskUtil.Await(() => _busControl.StartAsync());
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			_busControl?.Stop();
		}

		private IBusControl CreateBus()
		{
			return Bus.Factory.CreateUsingRabbitMq(x => x.Host(new Uri(Constants.RabbitMqUri), h =>
			{
				h.Username(Constants.UserName);
				h.Password(Constants.Password);
			}));
		}

		private async void button2_Click(object sender, EventArgs e)
		{
			try
			{
				var endpoint = await _busControl.GetSendEndpoint(new Uri($"{Constants.RabbitMqUri}{Constants.LoadOrderSagaServiceQueue}"));

				await endpoint.Send<IRequestLoadOrder>(new
				{
					DocumentKey = textBox1.Text,
					Timestamp = DateTime.UtcNow
				});
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
	}

	internal class LoadOrderRequest : ILoadOrderRequest
	{
		private readonly int _documentKey;
		private readonly DateTime _timestamp;

		public LoadOrderRequest(int documentKey)
		{
			_documentKey = documentKey;
			_timestamp = DateTime.UtcNow;
		}

		public DateTime Timestamp { get { return _timestamp; } }
		public int DocumentKey { get { return _documentKey; } }
	}
}