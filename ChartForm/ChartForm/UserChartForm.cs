using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChartForm
{
    public partial class UserChartForm: UserControl
    {
        private List<Point> PointList = new List<Point>(); // 用于存储输入的点位
        private List<string> seriesNames; // 存储线的名称
        public SeriesChartType SeriesType { get; set; }
        public Color SeriesColor { get; set; }
        public string SeriesName { get; set; }
        public int PointCount { get; set; }
        public double XMin { get; set; }
        public double XMax { get; set; }
        public double YMin { get; set; }
        public double YMax { get; set; }
        public int LineNum { get; set; }
        public UserChartForm()
        {
            InitializeComponent();
            seriesNames = new List<string>();
        }
        private void UserChartForm_Load(object sender, EventArgs e)
        {
            // 初始化图表
            InitializeChart();
        }
        private void InitializeChart()
        {
            chart1.MouseMove += chart1_MouseMove;
            chart1.MouseLeave += chart1_MouseLeave;
            chart1.Series.Clear(); // 清空默认的 Series
            // 设置图表标题
            chart1.Titles.Add("Chart");
            richTextBox1.Text = chart1.Titles[0].Text;
            chart1.Titles[0].Font = new Font("Arial", 12, FontStyle.Bold);
            Series minLineSeries = new Series("MinLine");
            Series maxLineSeries = new Series("MaxLine");
            minLineSeries.ChartType = SeriesChartType.Line;
            maxLineSeries.ChartType = SeriesChartType.Line;
            minLineSeries.Color = Color.Red;
            maxLineSeries.Color = Color.Red;
            chart1.Series.Add(minLineSeries);
            chart1.Series.Add(maxLineSeries);
            minLineSeries.IsVisibleInLegend = false;
            maxLineSeries.IsVisibleInLegend = false;
            // 显示X坐标轴和Y坐标轴
            chart1.ChartAreas[0].AxisX.Enabled = AxisEnabled.True;
            chart1.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
            // 设置X坐标轴的范围和间隔
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = 100;
            chart1.ChartAreas[0].AxisX.Interval = 10;
            // 设置Y坐标轴的范围和间隔
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = 100;
            chart1.ChartAreas[0].AxisY.Interval = 10;
            // 显示X轴和Y轴的刻度
            chart1.ChartAreas[0].AxisX.MajorTickMark.Enabled = true;
            chart1.ChartAreas[0].AxisY.MajorTickMark.Enabled = true;
            // 设置 X 坐标轴和 Y 坐标轴刻度的格式
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "0";
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "0";
            // 设置网格线
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            //
            comboBox1.DataSource = Enum.GetValues(typeof(SeriesChartType));
            comboBox1.SelectedItem = SeriesChartType.Line;
            comboBox2.BackColor = Color.Blue;
            comboBox2.DrawMode = DrawMode.OwnerDrawFixed;
            comboBox2.DrawItem += comboBox2_DrawItem;
            colorDialog1.Color = Color.Blue;
            textBox7.Visible = false;
            textBox7.KeyDown += textBox7_KeyDown;
            int LineNum = 0;
            textBox13.Text = "Line" + LineNum;
            listBox1.Visible = false;
            radioButton4.Checked = true;
            radioButton1.Checked = true;
            radioButton5.CheckedChanged += radioButton5_CheckedChanged;

        }
        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            // 获取鼠标在图表控件中的坐标
            Point mousePoint = new Point(e.X, e.Y);
            // 判断鼠标是否在折线上
            var hitResult = chart1.HitTest(mousePoint.X, mousePoint.Y);
            if (hitResult.ChartElementType == ChartElementType.DataPoint)
            {
                // 获取鼠标所在的坐标值
                double xValue = chart1.ChartAreas[0].AxisX.PixelPositionToValue(mousePoint.X);
                double yValue = chart1.ChartAreas[0].AxisY.PixelPositionToValue(mousePoint.Y);
                // 显示坐标值，限制为两位小数
                toolTip1.Show($"X: {xValue:F2}, Y: {yValue:F2}",
                    this, e.Location.X + 10, e.Location.Y - 15);
            }
            else
            {
                toolTip1.Hide(this);                // 鼠标不在折线上，隐藏ToolTip
            }
        }
        private void chart1_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Hide(this);
        }
        private void comboBox2_DrawItem(object sender, DrawItemEventArgs e)// 绘制背景色
        {
            if (e.Index >= 0)
            {
                e.DrawBackground();
                // 获取 ComboBox 中的颜色
                Color color = Color.FromName(comboBox1.Items[e.Index].ToString());
                // 创建用于绘制颜色的矩形区域
                Rectangle colorRect = new Rectangle(e.Bounds.Left + 2, e.Bounds.Top + 2, 20, e.Bounds.Height - 4);
                // 绘制颜色示例矩形
                using (SolidBrush brush = new SolidBrush(color))
                {
                    e.Graphics.FillRectangle(brush, colorRect);
                }
                // 绘制颜色名称
                using (SolidBrush brush = new SolidBrush(e.ForeColor))
                {
                    e.Graphics.DrawString(comboBox1.Items[e.Index].ToString(), e.Font, brush, colorRect.Right + 5, e.Bounds.Top + 2);
                }
                // 绘制焦点矩形框
                if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
                {
                    e.DrawFocusRectangle();
                }
            }
        }
        private void comboBox2_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                // 设置 ComboBox 的背景色为选择的颜色
                comboBox2.BackColor = colorDialog1.Color;
            }
        }
        private void Btn_add_Click(object sender, EventArgs e)//添加线
        {
            SeriesType = (SeriesChartType)comboBox1.SelectedItem;
            SeriesColor = colorDialog1.Color;
            SeriesName = textBox13.Text;
            PointCount = int.Parse(textBox12.Text);
            XMin = double.Parse(textBox11.Text);
            XMax = double.Parse(textBox10.Text);
            YMin = double.Parse(textBox9.Text);
            YMax = double.Parse(textBox8.Text);
            LineNum++;
            textBox13.Text = "Line" + LineNum;

            if (!string.IsNullOrEmpty(SeriesName))
            {
                seriesNames.Add(SeriesName);
                Lb_series.Items.Add(SeriesName);
            }
            // 创建新的 Series
            Series series = new Series(SeriesName);
            series.ChartType = SeriesType;
            series.Color = SeriesColor;

            if (IsRandomDataSelected())
            {
                // 生成随机数据点
                Random random = new Random();
                double step = (XMax - XMin) / (PointCount - 1);
                double xValue = XMin;

                for (int i = 0; i < PointCount; i++)
                {
                    double yValue = random.NextDouble() * (YMax - YMin) + YMin;
                    series.Points.AddXY(xValue, yValue);
                    xValue += step;
                }
            }
            else
            {
                foreach (Point point in PointList)
                {
                    series.Points.AddXY(point.X, point.Y);
                }
            }

            chart1.Series.Add(series);
        }
        private void Btn_delete_Click(object sender, EventArgs e)
        {
            // 删除选中线的数据
            if (Lb_series.SelectedIndex >= 0)
            {
                string selectedSeries = Lb_series.SelectedItem.ToString();
                seriesNames.Remove(selectedSeries);
                Lb_series.Items.Remove(selectedSeries);
                // 清除对应的 Series 数据
                Series seriesToRemove = null;
                foreach (Series series in chart1.Series)
                {
                    if (series.Name == selectedSeries)
                    {
                        seriesToRemove = series;
                        break;
                    }
                }
                // 如果找到了要删除的 Series，则移除它
                if (seriesToRemove != null)
                {
                    chart1.Series.Remove(seriesToRemove);
                }
                // 刷新图表
                chart1.Invalidate();
            }
        }
        private void Btn_title_Click(object sender, EventArgs e)
        {
            chart1.Titles[0].Text = richTextBox1.Text;
        }
        private void Btn_apply_Click(object sender, EventArgs e)
        {
            Axis selectedAxis;
            if (radioButton1.Checked)
                selectedAxis = chart1.ChartAreas[0].AxisY;
            else if (radioButton2.Checked)
                selectedAxis = chart1.ChartAreas[0].AxisX;
            else
                return;

            if (double.TryParse(textBox1.Text, out double minValue))
            {
                selectedAxis.Minimum = minValue;
            }
            else
            {
                MessageBox.Show("请输入有效的最小值。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (double.TryParse(textBox2.Text, out double maxValue))
            {
                selectedAxis.Maximum = maxValue;
            }
            else
            {
                MessageBox.Show("请输入有效的最大值。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (double.TryParse(textBox3.Text, out double interval))
            {
                selectedAxis.Interval = interval;
            }
            else
            {
                MessageBox.Show("请输入有效的轴刻度间隔。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string axisName = textBox4.Text.Trim();
            //if (string.IsNullOrEmpty(axisName))
            //{
            //    MessageBox.Show("请输入有效的坐标轴名称。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            selectedAxis.Title = axisName;

            selectedAxis.MajorGrid.Enabled = checkBox1.Checked;
            selectedAxis.IsStartedFromZero = checkBox2.Checked;

            // 重新绘制图表
            chart1.Invalidate();
        }
        private void Btn_value_Click(object sender, EventArgs e)
        {
            if (Lb_series.SelectedIndex >= 0)
            {
                string selectedSeries = Lb_series.SelectedItem.ToString();
                // 获取选中系列的最小值和最大值
                var selectedPoints = chart1.Series[selectedSeries].Points;
                double minValue = selectedPoints.Min(p => p.YValues[0]);
                double maxValue = selectedPoints.Max(p => p.YValues[0]);
                // 清除之前的线段数据
                chart1.Series["MinLine"].Points.Clear();
                chart1.Series["MaxLine"].Points.Clear();
                // 添加最小值线段的数据点
                chart1.Series["MinLine"].Points.AddXY(0, minValue);
                chart1.Series["MinLine"].Points.AddXY(selectedPoints[selectedPoints.Count - 1].XValue, minValue);
                // 添加最大值线段的数据点
                chart1.Series["MaxLine"].Points.AddXY(0, maxValue);
                chart1.Series["MaxLine"].Points.AddXY(selectedPoints[selectedPoints.Count - 1].XValue, maxValue);
                // 显示最小值和最大值
                textBox6.Text = minValue.ToString("F2");
                textBox5.Text = maxValue.ToString("F2");
            }
        }
        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string input = textBox7.Text.Trim();
                string[] values = input.Split(',');
                if (values.Length == 2 && int.TryParse(values[0], out int x) && int.TryParse(values[1], out int y))
                {
                    Point point = new Point(x, y);
                    PointList.Add(point);
                    listBox1.Items.Add($"{point.X}, {point.Y}");
                    textBox7.Clear();
                }
                //else
                //{
                //    MessageBox.Show("Invalid input format. Please enter valid point (x, y).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
            }
            textBox12.Text = PointList.Count.ToString();
        }
        public bool IsRandomDataSelected()
        {
            return radioButton4.Checked;
        }
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
            {
                textBox7.Visible = true;
                listBox1.Visible = true;
            }
            else
            {
                textBox7.Visible = false;
                listBox1.Visible = false;
            }
        }
    }
}
