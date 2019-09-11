using System;
using System.Collections.Generic;
using System.Text;

namespace FitMyFood.Views.CustomControls
{
    /*
    public class PieChart extends View
    {
    private class Item
    {
        public double itemValue;
        public int color;
        public int boundaryColor;

        public Item(double itemValue, int color, int boundaryColor)
        {
            this.itemValue = itemValue;
            this.color = color;
            this.boundaryColor = boundaryColor;
        }
    }

    private final static int BORDER_OFFSET = 2;
    private List<Item> items;
    private double itemsSum;
    private Paint paint;
    private RectF rectF;

    public PieChart(Context context)
    {
        super(context);
        paint = new Paint();
        rectF = new RectF();
        reset();
    }

    public PieChart(Context context, AttributeSet attrs)
    {
        super(context, attrs);
        paint = new Paint();
        rectF = new RectF();

        reset();
    }
    public void reset()
    {
        items = new ArrayList<>();
        itemsSum = 0;
    }
    public void addItem(double itemValue, int color, int boundaryColor)
    {
        items.add(new Item(itemValue, color, boundaryColor));
        itemsSum += itemValue;
    }

    public void setFoodItem(FoodItem target, FoodItem actual)
    {
        reset();
        int countOfMissing = 0;
        if (target.getCarbo() * target.getWeight() > actual.getCarbo() * actual.getWeight())
        {
            countOfMissing++;
        }
        if (target.getFat() * target.getWeight() > actual.getFat() * actual.getWeight())
        {
            countOfMissing++;
        }
        if (target.getProtein() * target.getWeight() > actual.getProtein() * actual.getWeight())
        {
            countOfMissing++;
        }


        if (target.getCarbo() * target.getWeight() > actual.getCarbo() * actual.getWeight())
        {
            addItem(CalculationDetails.ENERGYCARBO * actual.getCarbo() * actual.getWeight() / 100, ContextCompat.getColor(getContext(), R.color.colorChartCarbo), ContextCompat.getColor(getContext(), R.color.colorChartCarbo));
            addItem(((target.getEnergy() - actual.getEnergy()) / countOfMissing) / CalculationDetails.ENERGYCARBO, ContextCompat.getColor(getContext(), R.color.colorChartBackground), ContextCompat.getColor(getContext(), R.color.colorChartBackground));
        }
        else
        {
            addItem(CalculationDetails.ENERGYCARBO * target.getCarbo() * target.getWeight() / 100, ContextCompat.getColor(getContext(), R.color.colorChartCarbo), ContextCompat.getColor(getContext(), R.color.colorChartCarbo));
            addItem(CalculationDetails.ENERGYCARBO * (actual.getCarbo() * actual.getWeight() / 100 - target.getCarbo() * target.getWeight() / 100), ContextCompat.getColor(getContext(), R.color.colorChartCarbo), ContextCompat.getColor(getContext(), R.color.colorChartCarboOver));
        }
        if (target.getFat() * target.getWeight() > actual.getFat() * actual.getWeight())
        {
            addItem(CalculationDetails.ENERGYFAT * actual.getFat() * actual.getWeight() / 100, ContextCompat.getColor(getContext(), R.color.colorChartFat), ContextCompat.getColor(getContext(), R.color.colorChartFat));
            addItem(((target.getEnergy() - actual.getEnergy()) / countOfMissing) / CalculationDetails.ENERGYFAT, ContextCompat.getColor(getContext(), R.color.colorChartBackground), ContextCompat.getColor(getContext(), R.color.colorChartBackground));
        }
        else
        {
            addItem(CalculationDetails.ENERGYFAT * target.getFat() * target.getWeight() / 100, ContextCompat.getColor(getContext(), R.color.colorChartFat), ContextCompat.getColor(getContext(), R.color.colorChartFat));
            addItem(CalculationDetails.ENERGYFAT * (actual.getFat() * actual.getWeight() / 100 - target.getFat() * target.getWeight() / 100), ContextCompat.getColor(getContext(), R.color.colorChartFat), ContextCompat.getColor(getContext(), R.color.colorChartFatOver));
        }
        if (target.getProtein() * target.getWeight() > actual.getProtein() * actual.getWeight())
        {
            addItem(CalculationDetails.ENERGYPROTEIN * actual.getProtein() * actual.getWeight() / 100, ContextCompat.getColor(getContext(), R.color.colorChartProtein), ContextCompat.getColor(getContext(), R.color.colorChartProtein));
            addItem(((target.getEnergy() - actual.getEnergy()) / countOfMissing) / CalculationDetails.ENERGYPROTEIN, ContextCompat.getColor(getContext(), R.color.colorChartBackground), ContextCompat.getColor(getContext(), R.color.colorChartBackground));
        }
        else
        {
            addItem(CalculationDetails.ENERGYPROTEIN * target.getProtein() * target.getWeight() / 100, ContextCompat.getColor(getContext(), R.color.colorChartProtein), ContextCompat.getColor(getContext(), R.color.colorChartProtein));
            addItem(CalculationDetails.ENERGYPROTEIN * (actual.getProtein() * actual.getWeight() / 100 - target.getProtein() * target.getWeight() / 100), ContextCompat.getColor(getContext(), R.color.colorChartProtein), ContextCompat.getColor(getContext(), R.color.colorChartProteinOver));
        }
        invalidate();
    }


    public void onDraw(Canvas canvas)
    {
        paint.reset();
        paint.setAntiAlias(true);
        float angleIndex = 0;
        paint.setColor(ContextCompat.getColor(getContext(), R.color.colorChartBorder));
        float radius = canvas.getWidth() / 2;
        canvas.drawCircle(radius, radius, radius, paint);

        for (Item item : items)
        {
            if (item.itemValue > 0)
            {
                paint.setColor(item.color);
                float sweepAngle = (float)(item.itemValue / itemsSum * 360);
                rectF.set(BORDER_OFFSET, BORDER_OFFSET, canvas.getWidth() - BORDER_OFFSET, canvas.getHeight() - BORDER_OFFSET);
                if (item.color == item.boundaryColor)
                {
                    paint.setStrokeWidth(1);

                    paint.setStyle(Paint.Style.FILL_AND_STROKE);
                    paint.setColor(item.color);
                    canvas.drawArc(rectF, angleIndex, sweepAngle, true, paint);
                }
                else
                {
                    paint.setStrokeWidth(1);


                    paint.setStyle(Paint.Style.FILL);
                    paint.setColor(item.color);
                    canvas.drawArc(rectF, angleIndex, sweepAngle, true, paint);

                    paint.setStrokeWidth(2);
                    paint.setColor(item.boundaryColor);
                    paint.setStyle(Paint.Style.STROKE);
                    for (int i = 0; i <= (canvas.getHeight() - BORDER_OFFSET) / 2; i += 9)
                    {
                        rectF.set(BORDER_OFFSET + i, BORDER_OFFSET + i, canvas.getWidth() - BORDER_OFFSET - i, canvas.getHeight() - BORDER_OFFSET - i);
                        canvas.drawArc(rectF, angleIndex, sweepAngle, true, paint);

                    }
                }

                angleIndex += sweepAngle;

            }
        }
        */

    }
